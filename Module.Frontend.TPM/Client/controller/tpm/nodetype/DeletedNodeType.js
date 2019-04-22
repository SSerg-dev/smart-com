Ext.define('App.controller.tpm.nodetype.DeletedNodeType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletednodetype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletednodetype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletednodetype #datatable': {
                    activate: this.onActivateCard
                },
                'deletednodetype #detailform': {
                    activate: this.onActivateCard
                },
                'deletednodetype #detail': {
                    click: this.onDetailButtonClick
                },
                'deletednodetype #table': {
                    click: this.onTableButtonClick
                },
                'deletednodetype #prev': {
                    click: this.onPrevButtonClick
                },
                'deletednodetype #next': {
                    click: this.onNextButtonClick
                },
                'deletednodetype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletednodetype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletednodetype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletednodetype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
