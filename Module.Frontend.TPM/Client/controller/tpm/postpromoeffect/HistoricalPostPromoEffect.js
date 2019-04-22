Ext.define('App.controller.tpm.postpromoeffect.HistoricalPostPromoEffect', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpostpromoeffect directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpostpromoeffect directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpostpromoeffect #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpostpromoeffect #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpostpromoeffect #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpostpromoeffect #table': {
                    click: this.onTableButtonClick
                },
                'historicalpostpromoeffect #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpostpromoeffect #next': {
                    click: this.onNextButtonClick
                },
                'historicalpostpromoeffect #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpostpromoeffect #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpostpromoeffect #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
