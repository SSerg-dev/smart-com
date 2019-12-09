Ext.define('App.controller.tpm.promotypes.DeletedPromoTypes', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromotypes directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromotypes directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromotypes #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromotypes #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromotypes #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedpromotypes #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromotypes #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromotypes #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromotypes #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromotypes #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromotypes #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromotypes #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
