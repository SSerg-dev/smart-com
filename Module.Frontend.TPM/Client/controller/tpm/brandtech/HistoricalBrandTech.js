Ext.define('App.controller.tpm.brandtech.HistoricalBrandTech', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalbrandtech directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalbrandtech directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalbrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'historicalbrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'historicalbrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalbrandtech #table': {
                    click: this.onTableButtonClick
                },
                'historicalbrandtech #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalbrandtech #next': {
                    click: this.onNextButtonClick
                },
                'historicalbrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalbrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalbrandtech #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
