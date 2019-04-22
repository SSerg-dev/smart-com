Ext.define('App.controller.tpm.segment.HistoricalSegment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalsegment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalsegment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalsegment #datatable': {
                    activate: this.onActivateCard
                },
                'historicalsegment #detailform': {
                    activate: this.onActivateCard
                },
                'historicalsegment #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalsegment #table': {
                    click: this.onTableButtonClick
                },
                'historicalsegment #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalsegment #next': {
                    click: this.onNextButtonClick
                },
                'historicalsegment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalsegment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalsegment #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
