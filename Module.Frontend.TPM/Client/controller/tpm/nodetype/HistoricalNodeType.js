Ext.define('App.controller.tpm.nodetype.HistoricalNodeType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalnodetype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalnodetype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalnodetype #datatable': {
                    activate: this.onActivateCard
                },
                'historicalnodetype #detailform': {
                    activate: this.onActivateCard
                },
                'historicalnodetype #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalnodetype #table': {
                    click: this.onTableButtonClick
                },
                'historicalnodetype #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalnodetype #next': {
                    click: this.onNextButtonClick
                },
                'historicalnodetype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalnodetype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalnodetype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
