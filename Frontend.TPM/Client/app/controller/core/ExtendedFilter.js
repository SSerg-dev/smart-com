Ext.define('App.controller.core.ExtendedFilter', {
    extend: 'Ext.app.Controller',

    init: function () {
        this.listen({
            component: {
                'extfilter #apply': {
                    click: this.onApplyButtonClick
                },
                'extfilter #reject': {
                    click: this.onRejectButtonClick
                },
                'extfilter #cancel': {
                    click: this.onCancelButtonClick
                },
                'extfilter #efsettingsbutton': {
                    click: this.onSettingsButtonClick
                },
                'extfilter': {
                    show: this.onWindowShow,
                    resize: this.onWindowResize
                },
                'extselectionfilter': {
                    add: this.onSelectionFilterPanelChange,
                    remove: this.onSelectionFilterPanelChange
                },
                'extfilter #efselectionmodelbutton': {
                    click: this.onSelectionTypeMenuItemClick
                },
                'extfilter #eftextmodelbutton': {
                    click: this.onTextTypeMenuItemClick
                }
            }
        });
    },

    onSelectionTypeMenuItemClick: function () {
        var window = Ext.ComponentQuery.query('extfilter')[0],
            modelcontainer = window.down('#modelcontainer');

        modelcontainer.flex = 0;
        window.setHeight(null);

        window.getFilterContext().selectFilterModel('efselectionmodel');
    },

    onTextTypeMenuItemClick: function (menuitem) {
        var window = Ext.ComponentQuery.query('extfilter')[0],
            windowHeight = window.getHeight(),
            windowBodyHeight = window.body.getHeight(true),
            modelcontainer = window.down('#modelcontainer');

        window.getFilterContext().selectFilterModel('eftextmodel');

        if (windowBodyHeight > modelcontainer.down().minHeight) {
            modelcontainer.flex = 1;
            window.setHeight(windowHeight);
        }
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

    onApplyButtonClick: function (button) {
        var filterWindow = button.up('extfilter'),
            modelView = filterWindow.modelContainer.child();

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

        filterWindow.getFilterContext().commit();
        filterWindow.close();
    },

    onRejectButtonClick: function (button) {
        var filterWindow = button.up('extfilter');

        var selectionFilter = filterWindow.down('extselectionfilter');
        if (selectionFilter) {
            selectionFilter.focus();
        }

        filterWindow.getFilterContext().clear();   
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
        settingsWindow.center();
    }

});