Ext.define('App.controller.tpm.promotypes.HistoricalPromoTypes', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromotypes directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromotypes directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpromotypes #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromotypes #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromotypes #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpromotypes #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromotypes #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromotypes #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromotypes #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromotypes #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromotypes #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
