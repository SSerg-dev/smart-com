Ext.define('App.controller.core.ExtendedDetailFilter', {
    extend: 'Ext.app.Controller',

    init: function () {
        this.listen({
            component: {
                'extdetailfilter #detailapply': {
                    click: this.onDetailFilterApplyButtonClick
                },
                'extdetailfilter #reject': {
                    click: this.onDetailFilterRejectButtonClick
                },
                'extdetailfilter #cancel': {
                    click: this.onCancelButtonClick
                },
                'extdetailfilter #efsettingsbutton': {
                    click: this.onSettingsButtonClick
                },
                'extdetailfilter': {
                    show: this.onWindowShow,
                    resize: this.onWindowResize
                },
                'extdetailselectionfilter': {
                    add: this.onSelectionFilterPanelChange,
                    remove: this.onSelectionFilterPanelChange
                },
                'extdetailfilter #efselectionmodelbutton': {
                    click: this.onDetailSelectionTypeMenuItemClick
                },
                'extdetailfilter #eftextmodelbutton': {
                    click: this.onDetailTextTypeMenuItemClick
                },
                'extdetailfilter': {
                    beforedestroy: this.onDetailFilterBeforeDestroy 
                },

                'extmasterfilter #masterapply': {
                    click: this.onMasterFilterApplyButtonClick
                },
                'extmasterfilter #masterreject': {
                    click: this.onMasterRejectButtonClick
                }
            }
        });
    },

    onWindowShow: function (window) {
        this.constrainExtFilterWindow(window);
        var filterRow = window.down('extfilterrow');
        if (filterRow) {
            filterRow.focus();
        }
    },

    onWindowResize: function (window) {
        this.constrainExtFilterWindow(window);
        window.center();
    },

    onSelectionFilterPanelChange: function (panel) {
        var window = panel.up('window'),
            modelcontainer = window.down('#modelcontainer');

        if (modelcontainer.getHeight() < window.body.getHeight(true) && panel.getHeight() > panel.minHeight) {
            window.setHeight(null);
        }
    },

    constrainExtFilterWindow: function (window) {
        var maxHeight = Ext.getBody().getViewSize().height * 0.8; // 80%
        if (window.getHeight() > maxHeight) {
            window.setHeight(maxHeight >= window.minHeight ? maxHeight : window.minHeight);
        }
    },

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
            // Cохранение дитэйл-контекста
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

    onDetailTextTypeMenuItemClick: function (menuitem) {
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

    onDetailFilterRejectButtonClick: function (button) {
        var filterWindow = Ext.ComponentQuery.query('extmasterfilter')[0];
            detailFilterWindow = button.up('extdetailfilter'),
            topWindow = Ext.ComponentQuery.query('#mainwindow')[0],
            topGrid = topWindow.down('grid'),
            topGridStore = topGrid.getStore(),
            linkedWindow = Ext.ComponentQuery.query('#linkedwindow')[0],
            linkedGrid = linkedWindow.down('grid'),
            linkedStore = linkedGrid.getStore(),
            linkedModelEntityName = linkedStore.getProxy().getBreezeEntityType();

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
                    return item.entity ? item.entity != linkedModelEntityName : true;
                });
                topGridStoreExtendedFilter.filter.rules = newRules;
            }

            var toClose = false;
            masterFilterWindowApplyButton.fireEvent('click', toClose);
        }
    },

    onDetailSelectionTypeMenuItemClick: function (menuitem) {
        var window = menuitem.up('extdetailfilter'),
            modelcontainer = window.down('#modelcontainer');

        if (modelcontainer.flex != 0) {
            modelcontainer.flex = 0;
            window.setHeight(null);
        }

        window.getFilterContext().selectFilterModel('efselectionmodel');
    },

    onCancelButtonClick: function (button) {
        var filterWindow = button.up('extfilter');
        filterWindow.getFilterContext().reject();
        filterWindow.close();
    },

    onSettingsButtonClick: function (button) {
        var settingsWindow = Ext.widget('extfiltersettings').show(),
            settingsGrid = settingsWindow.down('#filtersettingsgrid'),
            settingsStore = settingsGrid.getStore(),
            filterWindow = button.up('window'),
            filterModel = filterWindow.getFilterContext().getFilterModel(),
            modelClassFullName = filterModel.getModel().getName(),
            modelName = App.Util.getClassNameWithoutNamespace(modelClassFullName),
            moduleName = App.Util.getSubdirectory(modelClassFullName),
            selectedFields = filterModel.getSelectedFields(),
            fields = filterModel.getFields().map(function (name) {
                return {
                    id: name,
                    name: l10n.ns(moduleName, modelName).value(name)
                }
            }, this);

        settingsStore.loadData(fields);
        settingsGrid.getSelectionModel().select(selectedFields.map(function (name) {
            return settingsStore.getById(name);
        }));
        settingsWindow.addListener('close', this.onSettingsClose);
    },

    onSettingsClose: function () {
        settingsWindow = Ext.ComponentQuery.query('extfiltersettings')[0];
        if (settingsWindow) {
            settingsWindow.close();
        }
    },

    onDetailFilterBeforeDestroy: function () {
        var masterFilterWindow = Ext.ComponentQuery.query('extmasterfilter')[0];
        if (masterFilterWindow) {
            masterFilterWindow.close();
        }
    },

    onMasterFilterApplyButtonClick: function (toClose) {
        var filterWindow = Ext.ComponentQuery.query('extmasterfilter')[0],
            linkedWindow = Ext.ComponentQuery.query('#linkedwindow')[0],
            linkedGrid = linkedWindow.down('grid'),
            linkedStore = linkedGrid.getStore(),
            linkedModelEntityName = linkedStore.getProxy().getBreezeEntityType();

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
                // Контекст дитэйл-фильтра
                var detailFilterContext = filterWindow.detailFilterContext;
                // Последний дитэйл-фильтр
                var lastDetailFilter = Ext.ComponentQuery.query('combineddirectorypanel')[0].detailFilter;
                var context = filterWindow.getFilterContext();
                var detailFilter;
                // Если был применён дитэйл-фильтр
                if (detailFilterContext) {
                    context.detailContext = detailFilterContext;
                    detailFilter = detailFilterContext.filter;
                    // Если был применён не пустой дитэйл-фильтр  (как например при очистке)
                    if (detailFilter) {
                        detailFilter.operator = 'any';
                        detailFilter.entity = linkedModelEntityName;
                        detailFilter.rules.map(function (rule) {
                            function makeNode(rule) {
                                if (rule) {
                                    if (rule.operator) {
                                        return rule.rules.map(function (item) {
                                            return makeNode.apply(this, [item]);
                                        }, this);
                                    } else if (rule) {
                                        rule.entity = linkedModelEntityName;
                                        return rule;
                                    }
                                }
                            };
                            return makeNode.apply(this, [rule]);
                        });
                        // Удалённые записи не учитывать при фильтрации (TODO: рассмотреть возможность реализации Condition Mapping || убрать реализацию IDeactivatable из PromoSupportPromo)
                        detailFilter.rules.push({
                            entity: linkedModelEntityName,
                            operation: "Equals",
                            property: "Disabled",
                            value: false
                        })
                    }
                    // Сохраняем фильтр
                    Ext.ComponentQuery.query('combineddirectorypanel')[0].detailFilter = detailFilter;
                }
                // Если дитэйл-фильтр уже был применён ранее, но не был модифицирован при повторном применении мастер-фильтра
                else if (lastDetailFilter)
                    detailFilter = lastDetailFilter;
                // Если есть дитэйл-фильтр, он добавляется к основному
                if (detailFilter) {
                    context.getFilterModel().commit();
                    var masterFilter = context.getFilterModel().getFilter();
                    if (masterFilter) {
                        masterFilter.rules.push(detailFilter);
                        //Если мастер-фильтр пуст
                    } else
                        masterFilter = detailFilter;
                    context.filter = masterFilter;
                    // Если дитэйл-фильтр пуст
                } else {
                    context.getFilterModel().commit();
                    context.filter = context.getFilterModel().getFilter();
                }
                context.reloadStore();
                context.fireEvent('extfilterchange', filterWindow.getFilterContext());

                if (toClose || toClose === undefined) {
                    filterWindow.close();
                }
            }
        }
    },

    onMasterRejectButtonClick: function (button) {
        var filterWindow = button.up('extmasterfilter'),
            topWindow = Ext.ComponentQuery.query('#mainwindow')[0],
            topGridStore = topWindow.down('grid').getStore(),
            topGridStoreExtendedFilter = topGridStore.extendedFilter;
            linkedWindow = Ext.ComponentQuery.query('#linkedwindow')[0],
            linkedGrid = linkedWindow.down('grid'),
            linkedStore = linkedGrid.getStore(),
            linkedModelEntityName = linkedStore.getProxy().getBreezeEntityType();

        if (filterWindow) {
            var masterFilterWindowApplyButton = filterWindow.down('#masterapply');
            var form = filterWindow.down('extselectionfilter'); // После очистки фильтра фокус остаётся на кнопке, перемещаем фокус на форму.

            if (form) {
                form.focus()
            }

            var newRules = []
            if (topGridStoreExtendedFilter.filter && topGridStoreExtendedFilter.filter.rules.length > 0) {
                newRules = topGridStoreExtendedFilter.filter.rules.filter(function (item) {
                    return item.entity == linkedModelEntityName;
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
                masterFilterWindowApplyButton.fireEvent('click', false);
            }
        }
    }
});