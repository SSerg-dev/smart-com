Ext.define('App.controller.tpm.brand.HistoricalBrand', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalbrand directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalbrand directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalbrand #datatable': {
                    activate: this.onActivateCard
                },
                'historicalbrand #detailform': {
                    activate: this.onActivateCard
                },
                'historicalbrand #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalbrand #table': {
                    click: this.onTableButtonClick
                },
                'historicalbrand #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalbrand #next': {
                    click: this.onNextButtonClick
                },
                'historicalbrand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalbrand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalbrand #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
