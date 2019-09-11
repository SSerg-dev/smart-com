Ext.define('App.controller.core.associatedmailnotificationsetting.recipient.AssociatedRecipient', {
    extend: 'App.controller.core.AssociatedDirectory',
	mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'associatedmailnotificationsettingrecipient directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'associatedmailnotificationsettingrecipient #datatable': {
                    activate: this.onActivateCard
                },
                'associatedmailnotificationsettingrecipient #detailform': {
                    activate: this.onActivateCard
                },               
                'associatedmailnotificationsettingrecipient #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'associatedmailnotificationsettingrecipient #detailform #next': {
                    click: this.onNextButtonClick
                },
                //

                'associatedmailnotificationsettingrecipient #detail': {
                    click: this.switchToDetailForm
                },
                'associatedmailnotificationsettingrecipient #table': {
                    click: this.onTableButtonClick
                },
                'associatedmailnotificationsettingrecipient #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'associatedmailnotificationsettingrecipient #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'associatedmailnotificationsettingrecipient #createbutton': {
                    click: this.onCreateButtonClick
                },
                'associatedmailnotificationsettingrecipient #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'associatedmailnotificationsettingrecipient #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'associatedmailnotificationsettingrecipient #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'associatedmailnotificationsettingrecipient #refresh': {
                    click: this.onRefreshButtonClick
                },
                'associatedmailnotificationsettingrecipient #close': {
                    click: this.onCloseButtonClick
                },

	            // import/export
                'associatedmailnotificationsettingrecipient #exportbutton': {
                    click: this.onExportButtonClick
                },
                'associatedmailnotificationsettingrecipient #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'associatedmailnotificationsettingrecipient #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'associatedmailnotificationsettingrecipient #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'associatedmailnotificationsettingrecipient #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                'associatedmailnotificationsettingrecipient #addbutton': {
                    click: this.onAddButtonClick
                },
            }
        });
    },
    getSaveModelConfig: function (record, grid) {
        var ownerGrid = grid.up('window').ownerGrid,
            parentPanel = ownerGrid.up('combineddirectorypanel').getParent();

        if (parentPanel) {
            return {
                MailNotificationSettingId: parentPanel.down('directorygrid').getSelectionModel().getSelection()[0].getId(),
                RoleId: record.getId()
            };
        }
    }
});