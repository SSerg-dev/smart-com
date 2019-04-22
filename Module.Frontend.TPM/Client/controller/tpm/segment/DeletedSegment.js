Ext.define('App.controller.tpm.segment.DeletedSegment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedsegment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedsegment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedsegment #datatable': {
                    activate: this.onActivateCard
                },
                'deletedsegment #detailform': {
                    activate: this.onActivateCard
                },
                'deletedsegment #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedsegment #table': {
                    click: this.onTableButtonClick
                },
                'deletedsegment #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedsegment #next': {
                    click: this.onNextButtonClick
                },
                'deletedsegment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedsegment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedsegment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedsegment #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
