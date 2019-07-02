Ext.define('App.controller.tpm.assortmentmatrix.DeletedAssortmentMatrix', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedassortmentmatrix directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedassortmentmatrix directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedassortmentmatrix #datatable': {
                    activate: this.onActivateCard
                },
                'deletedassortmentmatrix #detailform': {
                    activate: this.onActivateCard
                },
                'deletedassortmentmatrix #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedassortmentmatrix #table': {
                    click: this.onTableButtonClick
                },
                'deletedassortmentmatrix #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedassortmentmatrix #next': {
                    click: this.onNextButtonClick
                },
                'deletedassortmentmatrix #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedassortmentmatrix #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedassortmentmatrix #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedassortmentmatrix #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
