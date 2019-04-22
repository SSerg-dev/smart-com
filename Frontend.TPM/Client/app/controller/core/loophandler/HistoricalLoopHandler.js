Ext.define('App.controller.core.loophandler.HistoricalLoopHandler', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalloophandler directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalloophandler directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalloophandler #datatable': {
                    activate: this.onActivateCard
                },
                'historicalloophandler #detailform': {
                    activate: this.onActivateCard
                },
                'historicalloophandler #detail': {
                    click: this.switchToDetailForm
                },
                'historicalloophandler #table': {
                    click: this.onTableButtonClick
                },
                'historicalloophandler #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalloophandler #next': {
                    click: this.onNextButtonClick
                },
				//

                'historicalloophandler #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalloophandler #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalloophandler #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});