Ext.define('App.controller.tpm.nonenego.HistoricalNoneNego', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalnonenego directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalnonenego directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalnonenego #datatable': {
                    activate: this.onActivateCard
                },
                'historicalnonenego #detailform': {
                    activate: this.onActivateCard
                },
                'historicalnonenego #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalnonenego #table': {
                    click: this.onTableButtonClick
                },
                'historicalnonenego #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalnonenego #next': {
                    click: this.onNextButtonClick
                },
                'historicalnonenego #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalnonenego #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalnonenego #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});