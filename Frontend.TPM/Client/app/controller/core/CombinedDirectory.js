Ext.define('App.controller.core.CombinedDirectory', {
    extend: 'Ext.app.Controller',

    init: function () {
        this.listen({
            component: {
                '#detailfilter': {
                    click: this.onDetailFilterButtonClick
                },
            }
        });
    },

    getDefaultResource: function (button) {
        var result = null;
        var panel = button.up('combineddirectorypanel');
        if (panel) {
            var grid = panel.down('directorygrid');
            if (grid) {
                var proxy = grid.getStore().getProxy();
                result = proxy.resourceName;
            }
        }
        return result;
    },

    onExtFilterChange: function (ctx) {
        var grid = this.getSelectedGrid();
        if (grid) {
            var clearButton = grid.up('combineddirectorypanel').down('#extfilterclearbutton'),
                isFilterEmpty = ctx && ctx.isEmpty();

            if (ctx.store.model.$className != grid.getStore().model.$className)
                return;

            if (clearButton) {
                clearButton.setDisabled(isFilterEmpty);

                var text = isFilterEmpty
                    ? l10n.ns('core', 'filter').value('filterEmptyStatus')
                    : l10n.ns('core', 'filter').value('filterNotEmptyStatus');

                clearButton.setText(text);
                clearButton.setTooltip(text);
            }
        }
    },

    onGridAfterrender: function (grid) {
        var selModel = grid.getSelectionModel(),
            container = grid.up('combineddirectorypanel');

        this.updateButtonsState(container, selModel.hasSelection());
        this.updateViewSwitchButtonsState(container);
    },

    onGridStoreLoad: function (store, records) {
        var grid = this.getSelectedGrid();
        if (grid) {
            this.initSelection(grid, records);
        }
    },

    initSelection: function (grid, records) {
        var store = grid.getStore(),
            selModel = grid.getSelectionModel();

        if (!selModel.hasSelection() && records.length > 0 && store.data && store.data.length > 0) {
            selModel.select(0);
        } else if (selModel.hasSelection() && records.length > 0) {
            var selected = selModel.getSelection()[0];
            if (store.indexOfId(selected.getId()) === -1) {
                selModel.select(0);
            }
        } else if (records.length === 0) {
            selModel.deselectAll();
        }
    },

    onGridSelectionChange: function (selModel, selected) {
        var grid = selModel.view.up('grid'),
            container = grid.up('combineddirectorypanel');

        this.updateButtonsState(container, selModel.hasSelection());
        this.updateDetailForm(container, selModel);
    },

    onCreateButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            store = grid.getStore(),
            model = Ext.create(Ext.ModelManager.getModel(store.model));

        grid.editorModel.startCreateRecord(model);
    },

    onUpdateButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            grid.editorModel.startEditRecord(selModel.getSelection()[0]);
        } else {
            console.log('No selection');
        }
    },

    onDetailButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            grid.editorModel.startDetailRecord(selModel.getSelection()[0]);
        } else {
            console.log('No selection');
        }
    },

    onFilterButtonClick: function (button) {
        var store = this.getGridByButton(button).getStore();

        if (store.isExtendedStore) {
            Ext.widget('extfilter', store.getExtendedFilter()).show();
        } else {
            console.error('Extended filter does not implemented for this store');
        }
    },

    onDetailFilterButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            store = grid.getStore(),
            masterFilterWindow = Ext.widget('extmasterfilter', store.getExtendedFilter()),
            linkedWindow = Ext.ComponentQuery.query('#linkedwindow')[0],
            linkedGrid = linkedWindow.down('grid'),
            linkedStore = linkedGrid.getStore(),
            linkedModelName = linkedStore.model.modelName;

        masterFilterWindow.show();

        // Старый дитэйл-контекст из контекста мастер-фильтра
        ctx = masterFilterWindow.getFilterContext().detailContext,
        // Дитэйл-контекст, если перед повторным открытием дитэйл-фильтра, не был применён мастер-фильтр
        notImplementCtx = masterFilterWindow.detailFilterContext;
        if (notImplementCtx) {
            var win = Ext.widget('extdetailfilter', notImplementCtx);
        } else if (ctx) {
            var win = Ext.widget('extdetailfilter', ctx);
        } else {
            var storeConfig = {
                type: 'directorystore',
                model: linkedModelName,
                storeId: 'promosupportdetailfilterstore',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: linkedModelName,
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                }
            };
            // Если взять стор из грида, доп. фильтр будет работать некорректно
            var store = Ext.create('Ext.ux.data.ExtendedStore', storeConfig);
            if (store.isExtendedStore) {
                var win = Ext.widget('extdetailfilter', store.getExtendedFilter());
            } else {
                console.error('Extended filter does not implemented for this store');
            }
        }
        win.show();
    },

    onDeleteButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            Ext.Msg.show({
                title: l10n.ns('core').value('deleteWindowTitle'),
                msg: l10n.ns('core').value('deleteConfirmMessage'),
                fn: onMsgBoxClose,
                scope: this,
                icon: Ext.Msg.QUESTION,
                buttons: Ext.Msg.YESNO,
                buttonText: {
                    yes: l10n.ns('core', 'buttons').value('delete'),
                    no: l10n.ns('core', 'buttons').value('cancel')
                }
            });
        } else {
            console.log('No selection');
        }

        function onMsgBoxClose(buttonId) {
            if (buttonId === 'yes') {
                var record = selModel.getSelection()[0],
                    store = grid.getStore(),
                    view = grid.getView(),
                    currentIndex = store.indexOf(record),
                    pageIndex = store.getPageFromRecordIndex(currentIndex),
                    endIndex = store.getTotalCount() - 2; // 2, т.к. после удаления станет на одну запись меньше

                currentIndex = Math.min(Math.max(currentIndex, 0), endIndex);
                panel.setLoading(l10n.ns('core').value('deletingText'));

                record.destroy({
                    scope: this,
                    success: function () {
                        if (view.bufferedRenderer) {
                            selModel.deselectAll();
                            //selModel.clearSelections();
                            store.data.removeAtKey(pageIndex);
                            store.totalCount--;

                            if (store.getTotalCount() > 0) {
                                view.bufferedRenderer.scrollTo(currentIndex, true, function () {
                                    panel.setLoading(false);
                                });
                            } else {
                                grid.setLoadMaskDisabled(true);
                                store.on({
                                    single: true,
                                    load: function (records, operation, success) {
                                        panel.setLoading(false);
                                        grid.setLoadMaskDisabled(false);
                                    }
                                });
                                store.load();
                            }
                        } else {
                            panel.setLoading(false);
                        }
                    },
                    failure: function () {
                        panel.setLoading(false);
                    }
                });
            }
        }
    },

    getRecordId: function (record) {
        var idProperty = record.idProperty;
        return record.get(idProperty);
    },

    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = App.Util.buildViewClassName(panel, model, 'Historical');

            var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
            baseReviewWindow.show();

            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            if (proxy.extraParams) {
                proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);
            } else {
                proxy.extraParams = {
                    Id: this.getRecordId(selModel.getSelection()[0])
                }
            }

            store.setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(selModel.getSelection()[0])
            });
        }
    },

    onDeletedButtonClick: function (button) {
        this.createDeletedWindow(button).show();
    },

    onActivateCard: function (newCard, oldCard) {
        var container = newCard.up('combineddirectorypanel'),
            refreshBtn = container.down('#refresh');

        switch (newCard.getItemId()) {
            case 'datatable':
                if (refreshBtn) {
                    refreshBtn.show();
                }

                if (newCard.getView().bufferedRenderer) {
                    var selModel = newCard.getSelectionModel(),
                        store = newCard.getStore();

                    if (selModel.hasSelection()) {
                        var currentRecordIdx = this.getRecordId(selModel.getSelection()[0]);
                        newCard.getView().bufferedRenderer.scrollTo(store.indexOfId(currentRecordIdx));
                    }
                }
                break;
            case 'detailform':
                if (oldCard && refreshBtn) {
                    var selModel = oldCard.getSelectionModel();
                    if (selModel.hasSelection()) {
                        refreshBtn.hide();
                    }
                }
                break;
        }

        this.updateViewSwitchButtonsState(container);
    },

    switchToDetailForm: function (button) {
        var layout = button.up('combineddirectorypanel').getLayout();

        if (layout.type === 'card') {
            layout.setActiveItem('detailform');
        }
    },

    onTableButtonClick: function (button) {
        var layout = button.up('combineddirectorypanel').getLayout();

        if (layout.type === 'card') {
            layout.setActiveItem('datatable');
        }
    },

    onRefreshButtonClick: function (button) {
        var grid = button.up('combineddirectorypanel').down('#datatable');

        if (grid) {
            grid.getStore().load();
        }
    },

    onCloseButtonClick: function (button) {
        button.up('combineddirectorypanel').close();
    },

    onPrevButtonClick: function (button) {
        var panel = button.up('combineddirectorypanel'),
            grid = panel.down('#datatable');

        this.moveNextRecord(grid, -1);
    },

    onNextButtonClick: function (button) {
        var panel = button.up('combineddirectorypanel'),
            grid = panel.down('#datatable');

        this.moveNextRecord(grid, 1);
    },

    // Privates.

    updateDetailForm: function (container, selModel) {
        var form = container.down('#detailform');

        if (selModel.hasSelection()) {
            form.loadRecord(selModel.getSelection()[0]);
        } else {
            form.loadRecord(null);
        }
    },

    moveNextRecord: function (grid, direction) {
        var store = grid.getStore(),
            selModel = grid.getSelectionModel(),
            view = grid.getView(),
            currentRecord = null,
            nextIndex = 0,
            counter = 1;

        if (store.getTotalCount() == 0) {
            return;
        }

        if (selModel.hasSelection()) {
            currentRecord = selModel.getSelection()[0];
            nextIndex = store.indexOfId(this.getRecordId(currentRecord)) + Math.sign(direction);
        }

        if (nextIndex < 0) {
            nextIndex = 0;
        }

        store.getRange(nextIndex, nextIndex, {
            callback: function (range) {
                console.trace('getRange', arguments);
                selModel.select(range[0]);
            }
        });
    },

    getSelectedGrid: function () {
        return Ext.ComponentQuery.query('combineddirectorypanel[isSelected] directorygrid')[0];
    },

    getGridByButton: function (button) {
        return button.up('combineddirectorypanel').down('directorygrid');
    },

    updateButtonsState: function (container, isEnabled) {
        container.query('#updatebutton, #historybutton, #deletebutton, #detail')
            .forEach(function (button) {
                button.setDisabled(!isEnabled);
            });
    },

    updateViewSwitchButtonsState: function (container) {
        var layout = container.getLayout();

        if (layout.type === 'card') {
            var cardId = layout.getActiveItem().getItemId(),
                buttonIdCardIdMap = {
                    'detailform': 'detail',
                    'datatable': 'table'
                };
            container.query('#detail,#table').forEach(function (button) {
                if (button.getItemId() === buttonIdCardIdMap[cardId]) {
                    button.setUI('blue-pressed-button-toolbar-toolbar');
                } else {
                    button.setUI('gray-button-toolbar-toolbar');
                }
            });
        }
    },

    getResourceName: function (grid, prefix) {
        var resourceName = grid.getStore().getProxy().resourceName;
        return Ext.String.format('{0}{1}', prefix || '', resourceName);
    },

    createDeletedWindow: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            model = panel.getBaseModel(),
            viewClassName = App.Util.buildViewClassName(panel, model, 'Deleted');

        var window = Ext.widget('basereviewwindow', {
            items: Ext.create(viewClassName, {
                baseModel: model
            })
        });
        var deletedStore = window.down('directorygrid').getStore();
        //Устанавливаем существующие FixedFilters для стора удаленных записей
        var gridStoreFixedFilters = grid.getStore().fixedFilters;
        if (gridStoreFixedFilters)
            for (var filterKey in gridStoreFixedFilters)
                deletedStore.setFixedFilter(filterKey, gridStoreFixedFilters[filterKey]);
        return window;
    }
});