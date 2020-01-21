Ext.define('App.controller.tpm.promostatus.PromoStatus', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promostatus[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promostatus directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promostatus #datatable': {
                    activate: this.onActivateCard
                },
                'promostatus #detailform': {
                    activate: this.onActivateCard
                },
                'promostatus #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promostatus #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promostatus #detail': {
                    click: this.onDetailButtonClick
                },
                'promostatus #table': {
                    click: this.onTableButtonClick
                },
                'promostatus #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promostatus #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promostatus #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promostatus #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promostatuseditor #edit': {
                    click: this.windowEditorStartEdit
                },
                'promostatuseditor #canceledit': {
                    click: this.windowEditorCanselEdit
                },
                'promostatus #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promostatus #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promostatus #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promostatus #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promostatus #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promostatus #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promostatus #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promostatus #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
    onUpdateButtonClick: function () {
        this.callParent(arguments);
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        if (currentRole === 'SupportAdministrator') {

            var mechaniceditor = Ext.ComponentQuery.query('promostatuseditor')[0];
            var systemName = mechaniceditor.down('[name=SystemName]');
            systemName.setDisabled(true);
        }
    },
    windowEditorStartEdit: function () { 
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        if (currentRole === 'SupportAdministrator') {

            var mechaniceditor = Ext.ComponentQuery.query('promostatuseditor')[0];
            var systemName = mechaniceditor.down('[name=SystemName]');
            systemName.setDisabled(true);
        }

    },
    windowEditorCanselEdit: function () {
         var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        if (currentRole === 'SupportAdministrator') {

            var mechaniceditor = Ext.ComponentQuery.query('promostatuseditor')[0];
            var systemName = mechaniceditor.down('[name=SystemName]');
            systemName.setDisabled(false);
        }

    }
});
