Ext.define('App.controller.tpm.category.DeletedCategory', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcategory directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcategory directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcategory #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcategory #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcategory #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcategory #table': {
                    click: this.onTableButtonClick
                },
                'deletedcategory #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcategory #next': {
                    click: this.onNextButtonClick
                },
                'deletedcategory #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcategory #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcategory #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcategory #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
