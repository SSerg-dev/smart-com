Ext.define('App.controller.core.associateduser.dbuser.HistoricalAssociatedDbUser', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalassociateddbuseruser directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalassociateddbuseruser directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalassociateddbuseruser #datatable': {
                    activate: this.onActivateCard
                },
                'historicalassociateddbuseruser #detailform': {
                    activate: this.onActivateCard
                },
                'historicalassociateddbuseruser #detail': {
                    click: this.switchToDetailForm
                },
                'historicalassociateddbuseruser #table': {
                    click: this.onTableButtonClick
                },
                'historicalassociateddbuseruser #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalassociateddbuseruser #next': {
                    click: this.onNextButtonClick
                },
				//

                'historicalassociateddbuseruser #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalassociateddbuseruser #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalassociateddbuseruser #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});