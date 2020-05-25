Ext.define('App.controller.tpm.pricelist.PriceList', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'pricelist[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'pricelist directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'pricelist #datatable': {
                    activate: this.onActivateCard
                },
                'pricelist #detailform': {
                    activate: this.onActivateCard
                },
                'pricelist #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'pricelist #detailform #next': {
                    click: this.onNextButtonClick
                },
                'pricelist #detail': {
                    click: this.onDetailButtonClick
                },
                'pricelist #table': {
                    click: this.onTableButtonClick
                },
                'pricelist #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'pricelist #refresh': {
                    click: this.onRefreshButtonClick
                },
                'pricelist #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'pricelist #exportbutton': {
                    click: this.onExportButtonClick
                },
            }
        });
    }
});
