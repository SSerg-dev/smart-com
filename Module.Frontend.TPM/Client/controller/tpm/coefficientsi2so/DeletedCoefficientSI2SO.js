Ext.define('App.controller.tpm.coefficientsi2so.DeletedCoefficientSI2SO', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcoefficientsi2so directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcoefficientsi2so directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcoefficientsi2so #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcoefficientsi2so #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcoefficientsi2so #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcoefficientsi2so #table': {
                    click: this.onTableButtonClick
                },
                'deletedcoefficientsi2so #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcoefficientsi2so #next': {
                    click: this.onNextButtonClick
                },
                'deletedcoefficientsi2so #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcoefficientsi2so #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcoefficientsi2so #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcoefficientsi2so #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
