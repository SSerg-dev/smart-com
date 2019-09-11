Ext.define('App.controller.core.AssociatedDirectory', {
    extend: 'App.controller.core.CombinedDirectory',

    onGridAfterrender: function (grid) {
        var cdPanel = grid.up('combineddirectorypanel');

        if (!cdPanel.isMain()) {
            grid.getStore().on({
                scope: this,
                load: this.onChildGridStoreLoad,
                grid: grid
            });
        }

        this.callParent(arguments);
        this.updateChildrenGridButtonsState(cdPanel, false);
    },

    onChildGridStoreLoad: function (store, records, successful, eOpts) {
        var grid = eOpts.grid;

        if (grid) {
            this.initChildGridSelection(grid, records);
        }
    },

    initChildGridSelection: function (grid, records) {
        var store = grid.getStore(),
            selModel = grid.getSelectionModel();

        if (!selModel.hasSelection() && records.length > 0) {
            var record = store.getAt(0);
            selModel.selected.add(record);
            selModel.lastSelected = record;
            grid.getView().refreshSelection();
            selModel.fireEvent('selectionchange', selModel, [record]);
        } else if (selModel.hasSelection() && records.length > 0) {
            var selected = selModel.getSelection()[0];
            if (store.indexOfId(selected.getId()) === -1) {
                var record = store.getAt(0);
                selModel.deselectAll();
                selModel.selected.add(record);
                selModel.lastSelected = record;
                grid.getView().refreshSelection();
                selModel.fireEvent('selectionchange', selModel, [record]);
            }
        } else if (records.length === 0) {
            selModel.deselectAll();
        }
    },

    onGridSelectionChange: function (selModel, selected) {
        this.callParent(arguments);
        var me = this;
        var grid = selModel.view.up('directorygrid'),
            cdPanel = grid.up('combineddirectorypanel'),
            childrenPanels = cdPanel.getChildren();

        if (Ext.isEmpty(childrenPanels)) {
            return;
        }

        this.updateChildrenGridButtonsState(cdPanel, selModel.hasSelection());

        // Загрузка данных для дочерних таблиц, если они есть
        childrenPanels.forEach(function (item) {
            var childGrid = item.down('directorygrid'),
                store = childGrid.getStore(),
                filterbar = childGrid.getPlugin('filterbar'),
                cfg = cdPanel.linkConfig[item.getXType()];

            if (filterbar) {
                filterbar.clearFilters(true);
                //Дублирование метода onExtFilterChange в App.controller.core.CombinedDirectory 
                var clearButton = item.down('#extfilterclearbutton');
                if (clearButton) {
                    clearButton.setDisabled(true);
                    var text = true
                        ? l10n.ns('core', 'filter').value('filterEmptyStatus')
                        : l10n.ns('core', 'filter').value('filterNotEmptyStatus');
                    clearButton.setText(text);
                    clearButton.setTooltip(text);
                }
            }

            childGrid.getSelectionModel().deselectAll();
            store.clearAllFilters(true);

            if (selModel.hasSelection()) {
                store.setFixedFilter(cfg.detailField, {
                    property: cfg.detailField,
                    operation: 'Equals',
                    value: selected[0].get(cfg.masterField)
                });
            } else {
                store.loadData([]);
            }

            //Рекурсивно вызываем для текущих детей item
            me.onGridSelectionChange(childGrid.getSelectionModel(), childGrid.getSelectionModel().getSelection());

        });
    },

    onSelectorGridSelectionChange: function (selModel) {
        var window = selModel.view.up('window'),
            hasSelection = selModel.hasSelection();

        window.down('#select')[hasSelection ? 'enable' : 'disable']();
    },

    onAddButtonClick: function (button) {
        var cdPanel = this.getSelectorPanel();

        if (cdPanel) {
            Ext.widget('selectorwindow', {
                title: l10n.ns('core').value('selectorWindowTitle'),
                itemId: Ext.String.format('{0}_{1}_{2}', button.up('associateddirectoryview').getXType(), cdPanel.getXType(), 'selectorwindow'),
                ownerGrid: this.getGridByButton(button),
                items: [cdPanel]
            }).show();
        }
    },

    onSelectButtonClick: function (button) {
        var window = button.up('window'),
            ownerGrid = window.ownerGrid,
            grid = window.down('grid'),
            selModel = grid.getSelectionModel(),
            record = selModel.hasSelection() ? selModel.getSelection()[0] : null;

        if (record && ownerGrid) {
            var modelClass = Ext.ModelManager.getModel(ownerGrid.getStore().model),
                model = Ext.create(modelClass, this.getSaveModelConfig(record, grid));

            window.setLoading(l10n.ns('core').value('savingText'));

            model.save({
                scope: this,
                success: function () {
                    window.close();

                    ownerGrid.getStore().on({
                        single: true,
                        scope: this,
                        load: function (records, operation, success) {
                            model.set('Key');
                            window.setLoading(false);
                            if (typeof ownerGrid.afterSaveCallback === 'function') {
                                ownerGrid.afterSaveCallback(ownerGrid);
                            }
                        }
                    });

                    ownerGrid.getStore().load();
                    window.close();
                },
                failure: function () {
                    //model.reject();
                    window.setLoading(false);
                }
            });
        }
    },

    // Должен быть переопределён в наследнике, если есть кнопка add
    getSaveModelConfig: Ext.emptyFn,

    // Должен быть переопределён в наследнике, если есть кнопка add.
    // Возвращает панель для SelectorWindow.
    getSelectorPanel: Ext.emptyFn,

    onCreateButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            store = grid.getStore(),
            model = Ext.create(Ext.ModelManager.getModel(store.model)),
            cdPanel = grid.up('combineddirectorypanel'),
            parentPanel = cdPanel.getParent();
        this.scrollToDetailForm(cdPanel);
        // Если это дочерний грид, то в модель устанавливается значение masterField
        if (parentPanel) {
            var selModel = parentPanel.down('directorygrid').getSelectionModel();
            var masterKey = Object.keys(parentPanel.linkConfig)[0];
            var config = parentPanel.linkConfig[masterKey];

            if (selModel.hasSelection()) {
                model.set(config.detailField, selModel.getSelection()[0].get(config.masterField));
            }
        }

        grid.editorModel.startCreateRecord(model);
    },

    onUpdateButtonClick: function (button) {
        var selModel = this.getGridByButton(button).getSelectionModel();

        if (selModel.hasSelection()) {
            var cdPanel = button.up('combineddirectorypanel');
            this.scrollToDetailForm(cdPanel);
        }
        this.callParent(arguments);
    },

    scrollToDetailForm: function (cdPanel) {
        if (!cdPanel) {
            return;
        }

        var container = cdPanel.up();
        if (container.hasScroll) {
            var viewcontainer = container.up('#viewcontainer'),
                containerHeight,
                jspData
            // Если мастер-дитэйл, прокрутка во viewcontainer
            if (viewcontainer) {
                containerHeight = viewcontainer.getHeight();
                jspData = $(viewcontainer.getTargetEl().dom).data('jsp');
            } else {
                containerHeight = container.getHeight()
                jspData = $(container.getTargetEl().dom).data('jsp')
                // если не мастер-дитэйл вьюха, то недопрокручивается вниз на высоту тулбара
                if (container.header) {
                    containerHeight -= container.header.getHeight();
                }
            }
            var top = cdPanel.getY(),
                bottom = cdPanel.getRegion().bottom,
                delta = bottom - containerHeight,
                toptoolbarHeigth = 0;

            if (viewcontainer) {
                toptoolbarHeigth = viewcontainer.up().down('toptoolbar').getHeight();
                // Если не мастер-дитэйл вьюха, то недокручивается вверх 
            } else if (container.header) {
                toptoolbarHeigth = container.header.getHeight();
                toptoolbarHeigth += 30
            }
            toptoolbarHeigth += 10;
            if (top < 0) {
                jspData.scrollByY(top - toptoolbarHeigth);
            }

            if (delta > 0) {
                jspData.scrollByY(delta);
            }
        }
    },

    onDeletedButtonClick: function (button) {
        var panel = button.up('combineddirectorypanel'),
            parentPanel = panel.getParent(),
            window = this.createDeletedWindow(button).show();

        // Если это дочерний грид, то устанавливается фильтр по умолчанию
        if (parentPanel) {
            var selModel = parentPanel.down('directorygrid').getSelectionModel();
            var masterKey = Object.keys(parentPanel.linkConfig)[0];
            var config = parentPanel.linkConfig[masterKey];

            if (selModel.hasSelection()) {
                window.down('directorygrid')
                    .getStore()
                    .setFixedFilter(config.detailField, {
                        property: config.detailField,
                        operation: 'Equals',
                        value: selModel.getSelection()[0].get(config.masterField)
                    });
            }
        }
    },

    updateChildrenGridButtonsState: function (cdPanel, isEnabled) {
        var childrenPanels = cdPanel.getChildren();

        if (Ext.isEmpty(childrenPanels)) {
            return;
        }

        childrenPanels.forEach(function (item) {
            item.query('#createbutton,#addbutton,#deletedbutton')
                .forEach(function (button) {
                    button.setDisabled(!isEnabled);
                });
        });
    }
});