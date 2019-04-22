Ext.define('App.controller.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalassociatedaccesspointaccesspoint directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalassociatedaccesspointaccesspoint directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalassociatedaccesspointaccesspoint #datatable': {
                    activate: this.onActivateCard
                },
                'historicalassociatedaccesspointaccesspoint #detailform': {
                    activate: this.onActivateCard
                },
                'historicalassociatedaccesspointaccesspoint #detail': {
                    click: this.switchToDetailForm
                },
                'historicalassociatedaccesspointaccesspoint #table': {
                    click: this.onTableButtonClick
                },
                'historicalassociatedaccesspointaccesspoint #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalassociatedaccesspointaccesspoint #next': {
                    click: this.onNextButtonClick
                },
				//

                'historicalassociatedaccesspointaccesspoint #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalassociatedaccesspointaccesspoint #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalassociatedaccesspointaccesspoint #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});