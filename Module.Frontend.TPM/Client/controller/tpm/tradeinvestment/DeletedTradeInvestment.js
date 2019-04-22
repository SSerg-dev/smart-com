Ext.define('App.controller.tpm.tradeinvestment.DeletedTradeInvestment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedtradeinvestment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedtradeinvestment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedtradeinvestment #datatable': {
                    activate: this.onActivateCard
                },
                'deletedtradeinvestment #detailform': {
                    activate: this.onActivateCard
                },
                'deletedtradeinvestment #detail': {
                    click: this.switchToDetailForm
                },
                'deletedtradeinvestment #table': {
                    click: this.onTableButtonClick
                },
                'deletedtradeinvestment #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedtradeinvestment #next': {
                    click: this.onNextButtonClick
                },
                'deletedtradeinvestment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedtradeinvestment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedtradeinvestment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedtradeinvestment #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
