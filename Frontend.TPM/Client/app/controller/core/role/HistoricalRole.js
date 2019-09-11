Ext.define('App.controller.core.role.HistoricalRole', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalrole directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalrole directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalrole #datatable': {
                    activate: this.onActivateCard
                },
                'historicalrole #detailform': {
                    activate: this.onActivateCard
                },
                'historicalrole #detail': {
                    click: this.switchToDetailForm
                },
                'historicalrole #table': {
                    click: this.onTableButtonClick
                },
                'historicalrole #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalrole #next': {
                    click: this.onNextButtonClick
                },
                //

                'historicalrole #extfilterbutton': {
                    click: this.onFilterButtonClick
                },

                'historicalrole #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalrole #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});