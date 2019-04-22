Ext.define('App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'associatedmailnotificationsettingmailnotificationsetting[issearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'associatedmailnotificationsettingmailnotificationsetting directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'associatedmailnotificationsettingmailnotificationsetting #datatable': {
                    activate: this.onActivateCard
                },
                'associatedmailnotificationsettingmailnotificationsetting #detailform': {
                    activate: this.onActivateCard
                },
                'associatedmailnotificationsettingmailnotificationsetting #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #detailform #next': {
                    click: this.onNextButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #detail': {
                    click: this.switchToDetailForm
                },
                'associatedmailnotificationsettingmailnotificationsetting #table': {
                    click: this.onTableButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #createbutton': {
                    click: this.onCreateButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'associatedmailnotificationsettingmailnotificationsetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'associatedmailnotificationsettingmailnotificationsetting #close': {
                    click: this.onCloseButtonClick
                },
            }
        });
    }
});