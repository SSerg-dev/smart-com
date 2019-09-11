Ext.define('App.controller.core.associatedmailnotificationsetting.recipient.HistoricalAssociatedRecipient', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalassociatedmailnotificationsettingrecipient directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalassociatedmailnotificationsettingrecipient directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalassociatedmailnotificationsettingrecipient #datatable': {
                    activate: this.onActivateCard
                },
                'historicalassociatedmailnotificationsettingrecipient #detailform': {
                    activate: this.onActivateCard
                },
                'historicalassociatedmailnotificationsettingrecipient #detail': {
                    click: this.switchToDetailForm
                },
                'historicalassociatedmailnotificationsettingrecipient #table': {
                    click: this.onTableButtonClick
                },
                'historicalassociatedmailnotificationsettingrecipient #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalassociatedmailnotificationsettingrecipient #next': {
                    click: this.onNextButtonClick
                },
				//

                'historicalassociatedmailnotificationsettingrecipient #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalassociatedmailnotificationsettingrecipient #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalassociatedmailnotificationsettingrecipient #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});