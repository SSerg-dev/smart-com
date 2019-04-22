Ext.define('App.controller.tpm.format.DeletedFormat', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedformat directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedformat directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedformat #datatable': {
                    activate: this.onActivateCard
                },
                'deletedformat #detailform': {
                    activate: this.onActivateCard
                },
                'deletedformat #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedformat #table': {
                    click: this.onTableButtonClick
                },
                'deletedformat #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedformat #next': {
                    click: this.onNextButtonClick
                },
                'deletedformat #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedformat #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedformat #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedformat #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
