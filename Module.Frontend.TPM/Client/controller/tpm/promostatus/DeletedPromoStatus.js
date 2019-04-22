Ext.define('App.controller.tpm.promostatus.DeletedPromoStatus', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromostatus directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromostatus directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromostatus #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromostatus #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromostatus #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedpromostatus #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromostatus #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromostatus #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromostatus #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromostatus #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromostatus #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromostatus #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
