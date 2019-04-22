Ext.define('App.controller.tpm.promodemand.HistoricalPromoDemand', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromodemand directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromodemand directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpromodemand #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromodemand #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromodemand #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpromodemand #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromodemand #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromodemand #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromodemand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromodemand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromodemand #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
