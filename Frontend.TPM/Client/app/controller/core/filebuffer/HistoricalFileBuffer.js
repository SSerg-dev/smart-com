Ext.define('App.controller.core.filebuffer.HistoricalFileBuffer', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalfilebuffer directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalfilebuffer directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalfilebuffer #datatable': {
                    activate: this.onActivateCard
                },
                'historicalfilebuffer #detailform': {
                    activate: this.onActivateCard
                },
                'historicalfilebuffer #detail': {
                    click: this.switchToDetailForm
                },
                'historicalfilebuffer #table': {
                    click: this.onTableButtonClick
                },
                'historicalfilebuffer #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalfilebuffer #next': {
                    click: this.onNextButtonClick
                },
				//

                'historicalfilebuffer #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalfilebuffer #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalfilebuffer #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});