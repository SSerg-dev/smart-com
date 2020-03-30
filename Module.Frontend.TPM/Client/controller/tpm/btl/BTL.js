Ext.define('App.controller.tpm.btl.BTL', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'btl[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'btl directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'btl #datatable': {
                    activate: this.onActivateCard
                },
                'btl #detailform': {
                    activate: this.onActivateCard
                },
                'btl #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'btl #detailform #next': {
                    click: this.onNextButtonClick
                },
                'btl #detail': {
                    click: this.onDetailButtonClick
                },
                'btl #table': {
                    click: this.onTableButtonClick
                },
                'btl #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'btl #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'btl #createbutton': {
                    click: this.onCreateButtonClick
                },
                'btl #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'btl #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'btl #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'btl #refresh': {
                    click: this.onRefreshButtonClick
                },
                'btl #close': {
                    click: this.onCloseButtonClick
                },

                // import/export
                'btl #exportbutton': {
                    click: this.onExportButtonClick
                },
                'btl #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'btl #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'btl #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
            }
        });
    },

    // Открывается окно дитэйл-фильтра
    onDetailFilterButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            store = grid.getStore(),
            masterFilterWindow = Ext.widget('extmasterfilter', store.getExtendedFilter());

        masterFilterWindow.show();

        // старый дитэйл-контекст из контекста мастер-фильтра
        ctx = masterFilterWindow.getFilterContext().detailContext,
        // дитэйл-контекст, если перед повторным открытием дитэйл-фильтра, не был применён мастер-фильтр
        notImplementCtx = masterFilterWindow.detailFilterContext;
        // если контекст уже есть, передаём его в конструктор окна фильтра
        if (notImplementCtx) {
            var win = Ext.widget('extdetailfilter', notImplementCtx);
        } else if (ctx) {
            var win = Ext.widget('extdetailfilter', ctx);
        } else {
            // конфиг стора для дитэйл-фильтра
            var storeConfig = {
                type: 'directorystore',
                model: 'App.model.tpm.btl.BTLPromo',
                storeId: 'btldetailfilterstore',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.btl.BTLPromo',
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                }
            };
            // создаём новый стор для дитэйл-фильтра. Если взять стор из грида, доп. фильтр будет работать некорректно
            var store = Ext.create('Ext.ux.data.ExtendedStore', storeConfig);
            if (store.isExtendedStore) {
                var win = Ext.widget('extdetailfilter', store.getExtendedFilter());
            } else {
                console.error('Extended filter does not implemented for this store');
            }
        }
        win.show();
    },
    
    onDetailFilterBeforeDestroy: function () {
        var masterFilterWindow = Ext.ComponentQuery.query('extmasterfilter')[0];
        if (masterFilterWindow) {
            masterFilterWindow.close();
        }
    },

    // Очищаем detail фильтр
    onDetailFilterRejectButtonClick: function (button) {
        var filterWindow = Ext.ComponentQuery.query('extmasterfilter')[0];
        var detailFilterWindow = button.up('extdetailfilter');
        // Нафига вы запихали это в LoopHandler?)
        var combinedDirectoryPanel = Ext.ComponentQuery.query('combineddirectorypanel')[0];
        var topGrid = (Ext.ComponentQuery.query('btlpromo')[0]).down('grid');
        var topGridStore = topGrid.getStore();
        var topGridStoreExtendedFilter = topGridStore.extendedFilter;
        var masterFilterWindowApplyButton = filterWindow.down('#masterapply');

        if (filterWindow && detailFilterWindow) {
            var form = filterWindow.down('extselectionfilter'); // После очистки фильтра фокус остаётся на кнопке, перемещаем фокус на форму.
            if (form) {
                form.focus()
            }

            filterWindow.getFilterContext().detailContext = null;
            filterWindow.detailFilterContext = null;
            Ext.ComponentQuery.query('combineddirectorypanel')[0].detailFilter = null;

            var newRules = [];

            if (topGridStoreExtendedFilter.filter && topGridStoreExtendedFilter.filter.rules.length > 0) {
                newRules = topGridStoreExtendedFilter.filter.rules.filter(function (item) {
                    return item.entity ? item.entity != 'BTLPromo' : true;
                });
                topGridStoreExtendedFilter.filter.rules = newRules;
            }

            masterFilterWindowApplyButton.fireEvent('click');
            detailFilterWindow.close();
        }
    },

    // Применение дитэйл-фильтра
    onDetailFilterApplyButtonClick: function (button) {
        var filterWindow = button.up('extdetailfilter'),
            masterFilterWindow = Ext.ComponentQuery.query('extmasterfilter')[0];

        if (masterFilterWindow) {
            var modelView = filterWindow.modelContainer.child(),
                masterFilterWindowApplyButton = masterFilterWindow.down('#masterapply');

            if (modelView) {
                if (!modelView.getForm().isValid()) {
                    Ext.Msg.show({
                        title: l10n.ns('core').value('errorTitle'),
                        msg: l10n.ns('core', 'filter').value('filterErrorMessage'),
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.Msg.ERROR
                    });
                    return;
                }
                modelView.commitChanges();
            }
            // сохранение дитэйл-контекста
            var context = filterWindow.getFilterContext();
            var model = context.getFilterModel();
            model.commit();

            context.filter = model.getFilter();
            context.fireEvent('extfilterchange', context);
            masterFilterWindow.detailFilterContext = context;
            masterFilterWindowApplyButton.fireEvent('click');

            filterWindow.close();
        }
    },
    // необходим т.к. базовый метод открывает текстовый фильтр только для первого окна TODO: исправить в ядре?
    ondetailTextTypeMenuItemClick: function (menuitem) {
        var window = menuitem.up('extdetailfilter'),
            windowHeight = window.getHeight(),
            windowBodyHeight = window.body.getHeight(true),
            modelcontainer = window.down('#modelcontainer');
        window.getFilterContext().selectFilterModel('eftextmodel');

        if (windowBodyHeight > modelcontainer.down().minHeight) {
            modelcontainer.flex = 1;
            window.setHeight(windowHeight);
        }
    },
    // необходим т.к. базовый метод закрывает текстовый фильтр только для первого окна TODO: исправить в ядре?
    ondetailSelectionTypeMenuItemClick: function (menuitem) {
        var window = menuitem.up('extdetailfilter'),
            modelcontainer = window.down('#modelcontainer');

        if (modelcontainer.flex != 0) {
            modelcontainer.flex = 0;
            window.setHeight(null);
        }

        window.getFilterContext().selectFilterModel('efselectionmodel');
    },
    // Применение мастер-фильтра
    onMasterFilterApplyButtonClick: function (menuitem) {
        var filterWindow = Ext.ComponentQuery.query('extmasterfilter')[0];

        if (filterWindow) {
            var modelView = filterWindow.modelContainer.child();

            if (modelView) {
                if (!modelView.getForm().isValid()) {
                    Ext.Msg.show({
                        title: l10n.ns('core').value('errorTitle'),
                        msg: l10n.ns('core', 'filter').value('filterErrorMessage'),
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.Msg.ERROR
                    });
                    return;
                }
                modelView.commitChanges();
                // контекст дитэйл-фильтра
                var detailFilterContext = filterWindow.detailFilterContext;
                // последний дитэйл-фильтр
                var lastDetailFilter = Ext.ComponentQuery.query('combineddirectorypanel')[0].detailFilter;
                var context = filterWindow.getFilterContext();
                var detailFilter;
                // если был применён дитэйл-фильтр
                if (detailFilterContext) {
                    context.detailContext = detailFilterContext;
                    detailFilter = detailFilterContext.filter;
                    // Если был применён не пустой дитэйл-фильтр  (как например при очистке)
                    if (detailFilter) {
                        detailFilter.operator = 'any';
                        detailFilter.entity = 'BTLPromo';
                        detailFilter.rules.map(function (rule) {
                            function makeNode(rule) {
                                if (rule) {
                                    if (rule.operator) {
                                        return rule.rules.map(function (item) {
                                            return makeNode.apply(this, [item]);
                                        }, this);
                                    } else if (rule) {
                                        rule.entity = "BTLPromo";
                                        return rule;
                                    }
                                }
                            };
                            return makeNode.apply(this, [rule]);
                        });
                        // Удалённые записи не учитывать при фильтрации (TODO: рассмотреть возможность реализации Condition Mapping || убрать реализацию IDeactivatable из PromoSupportPromo)
                        detailFilter.rules.push({
                            entity: "BTLPromo",
                            operation: "Equals",
                            property: "Disabled",
                            value: false
                        })
                    }
                    // сохраняем фильтр
                    Ext.ComponentQuery.query('combineddirectorypanel')[0].detailFilter = detailFilter;
                }
                // если дитэйл-фильтр уже был применён ранее, но не был модифицирован при повторном применении мастер-фильтра
                else if (lastDetailFilter)
                    detailFilter = lastDetailFilter;
                // если есть дитэйл-фильтр, он добавляется к основному
                if (detailFilter) {
                    context.getFilterModel().commit();
                    var masterFilter = context.getFilterModel().getFilter();
                    if (masterFilter) {
                        masterFilter.rules.push(detailFilter);
                        // если мастер-фильтр пуст
                    } else
                        masterFilter = detailFilter;
                    context.filter = masterFilter;
                    // если дитэйл-фильтр пуст
                } else {
                    context.getFilterModel().commit();
                    context.filter = context.getFilterModel().getFilter();
                }
                context.reloadStore();
                context.fireEvent('extfilterchange', filterWindow.getFilterContext());
                filterWindow.close();
            }
        }
    },
    // Очистка главного фильтра. Дополнительно очищается дитэйл-фильтр
    onMasterRejectButtonClick: function (button) {
        var filterWindow = button.up('extmasterfilter');
        var topGrid = (Ext.ComponentQuery.query('btlpromo')[0]).down('grid');
        var topGridStore = topGrid.getStore();
        var topGridStoreExtendedFilter = topGridStore.extendedFilter;

        if (filterWindow) {
            var masterFilterWindowApplyButton = filterWindow.down('#masterapply');
            var form = filterWindow.down('extselectionfilter'); // После очистки фильтра фокус остаётся на кнопке, перемещаем фокус на форму.

            if (form) {
                form.focus()
            }

            var newRules = []
            if (topGridStoreExtendedFilter.filter && topGridStoreExtendedFilter.filter.rules.length > 0) {
                newRules = topGridStoreExtendedFilter.filter.rules.filter(function (item) {
                    return item.entity == 'BTLPromo';
                });
            }

            // Если detail фильтр пустой, чистим все, иначе оставляем только правила detail фильтра.
            if (newRules.length == 0) {
                filterWindow.getFilterContext().clear();
            } else {
                var model = filterWindow.getFilterContext().getFilterModel();
                if (model) {
                    model.clear();
                }

                topGridStoreExtendedFilter.filter.rules = newRules;
                masterFilterWindowApplyButton.fireEvent('click');
            }
        }
    },
});
