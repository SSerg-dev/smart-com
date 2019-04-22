Ext.define('App.controller.tpm.region.DeletedRegion', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedregion directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedregion directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedregion #datatable': {
                    activate: this.onActivateCard
                },
                'deletedregion #detailform': {
                    activate: this.onActivateCard
                },
                'deletedregion #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedregion #table': {
                    click: this.onTableButtonClick
                },
                'deletedregion #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedregion #next': {
                    click: this.onNextButtonClick
                },
                'deletedregion #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedregion #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedregion #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedregion #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
