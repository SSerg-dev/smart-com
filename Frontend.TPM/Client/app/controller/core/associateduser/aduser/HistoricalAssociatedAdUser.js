Ext.define('App.controller.core.associateduser.aduser.HistoricalAssociatedAdUser', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalassociatedaduseruser directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalassociatedaduseruser directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalassociatedaduseruser #datatable': {
                    activate: this.onActivateCard
                },
                'historicalassociatedaduseruser #detailform': {
                    activate: this.onActivateCard
                },
                'historicalassociatedaduseruser #detail': {
                    click: this.switchToDetailForm
                },
                'historicalassociatedaduseruser #table': {
                    click: this.onTableButtonClick
                },
                'historicalassociatedaduseruser #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalassociatedaduseruser #next': {
                    click: this.onNextButtonClick
                },
				//

                'historicalassociatedaduseruser #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalassociatedaduseruser #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalassociatedaduseruser #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});