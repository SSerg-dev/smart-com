Ext.define('App.controller.core.ExtendedFilterSettings', {
    extend: 'Ext.app.Controller',

    init: function () {
        this.listen({
            component: {
                'extfiltersettings #ok': {
                    click: this.onApplySettingsButtonClick
                },
                'extfiltersettings #cancel': {
                    click: this.onCancelButtonClick
                }
            }
        });
    },

    onCancelButtonClick: function (button) {
        button.up('extfiltersettings').close();
    },

    onApplySettingsButtonClick: function (button) {
        var filterWindow = Ext.ComponentQuery.query('extfilter')[0],
            settingsWindow = button.up('window'),
            selModel = settingsWindow.down('grid').getSelectionModel(),
            filterModel = filterWindow.filterContext.getFilterModel(),
            newNames;

        if (!selModel.hasSelection()) {
            Ext.Msg.show({
                title: l10n.ns('core').value('errorTitle'),
                msg: l10n.ns('core', 'filter').value('settingsErrorMessage'),
                buttons: Ext.MessageBox.OK,
                icon: Ext.Msg.ERROR
            });
            return;
        }

        newNames = selModel.getSelection().map(function (model) {
            return model.getId();
        });
        filterModel.selectFields(newNames);
        settingsWindow.close();
    }

});