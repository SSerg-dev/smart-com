Ext.define('App.controller.tpm.sfatype.DeletedSFAType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedsfatype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedsfatype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedsfatype #datatable': {
                    activate: this.onActivateCard
                },
                'deletedsfatype #detailform': {
                    activate: this.onActivateCard
                },
                'deletedsfatype #detail': {
                    click: this.switchToDetailForm
                },
                'deletedsfatype #table': {
                    click: this.onTableButtonClick
                },
                'deletedsfatype #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedsfatype #next': {
                    click: this.onNextButtonClick
                },
                'deletedsfatype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedsfatype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedsfatype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedsfatype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
