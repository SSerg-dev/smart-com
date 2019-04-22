Ext.define('App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalassociatedmailnotificationsettingmailnotificationsetting directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalassociatedmailnotificationsettingmailnotificationsetting directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalassociatedmailnotificationsettingmailnotificationsetting #datatable': {
                    activate: this.onActivateCard
                },
                'historicalassociatedmailnotificationsettingmailnotificationsetting #detailform': {
                    activate: this.onActivateCard
                },
                'historicalassociatedmailnotificationsettingmailnotificationsetting #detail': {
                    click: this.switchToDetailForm
                },
                'historicalassociatedmailnotificationsettingmailnotificationsetting #table': {
                    click: this.onTableButtonClick
                },
                'historicalassociatedmailnotificationsettingmailnotificationsetting #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalassociatedmailnotificationsettingmailnotificationsetting #next': {
                    click: this.onNextButtonClick
                },
				//

                'historicalassociatedmailnotificationsettingmailnotificationsetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalassociatedmailnotificationsettingmailnotificationsetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalassociatedmailnotificationsettingmailnotificationsetting #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});