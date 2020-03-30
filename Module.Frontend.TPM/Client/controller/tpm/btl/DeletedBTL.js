Ext.define('App.controller.tpm.btl.DeletedBTL', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedbtl directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedbtl directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedbtl #datatable': {
                    activate: this.onActivateCard
                },
                'deletedbtl #detailform': {
                    activate: this.onActivateCard
                },
                'deletedbtl #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedbtl #table': {
                    click: this.onTableButtonClick
                },
                'deletedbtl #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedbtl #next': {
                    click: this.onNextButtonClick
                },
                'deletedbtl #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedbtl #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedbtl #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedbtl #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
