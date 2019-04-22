Ext.define('App.controller.tpm.tradeinvestment.HistoricalTradeInvestment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicaltradeinvestment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicaltradeinvestment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicaltradeinvestment #datatable': {
                    activate: this.onActivateCard
                },
                'historicaltradeinvestment #detailform': {
                    activate: this.onActivateCard
                },
                'historicaltradeinvestment #detail': {
                    click: this.switchToDetailForm
                },
                'historicaltradeinvestment #table': {
                    click: this.onTableButtonClick
                },
                'historicaltradeinvestment #prev': {
                    click: this.onPrevButtonClick
                },
                'historicaltradeinvestment #next': {
                    click: this.onNextButtonClick
                },
                'historicaltradeinvestment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicaltradeinvestment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicaltradeinvestment #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
