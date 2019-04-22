Ext.define('App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.DeletedAssociatedMailNotificationSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedassociatedmailnotificationsettingmailnotificationsetting directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'deletedassociatedmailnotificationsettingmailnotificationsetting #datatable': {
                    activate: this.onActivateCard
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting #detailform': {
                    activate: this.onActivateCard
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting #detail': {
                    click: this.switchToDetailForm
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting #table': {
                    click: this.onTableButtonClick
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting #next': {
                    click: this.onNextButtonClick
                },
                //

                'deletedassociatedmailnotificationsettingmailnotificationsetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedassociatedmailnotificationsettingmailnotificationsetting #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});