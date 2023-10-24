Ext.define('App.controller.tpm.sfatype.HistoricalSFAType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalsfatype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalsfatype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalsfatype #datatable': {
                    activate: this.onActivateCard
                },
                'historicalsfatype #detailform': {
                    activate: this.onActivateCard
                },
                'historicalsfatype #detail': {
                    click: this.switchToDetailForm
                },
                'historicalsfatype #table': {
                    click: this.onTableButtonClick
                },
                'historicalsfatype #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalsfatype #next': {
                    click: this.onNextButtonClick
                },
                'historicalsfatype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalsfatype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalsfatype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
