Ext.define('App.controller.core.setting.HistoricalSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalsetting directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalsetting directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'historicalsetting #datatable': {
                    activate: this.onActivateCard
                },
                'historicalsetting #detailform': {
                    activate: this.onActivateCard
                },
                'historicalsetting #detail': {
                    click: this.switchToDetailForm
                },
                'historicalsetting #table': {
                    click: this.onTableButtonClick
                },
                'historicalsetting #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalsetting #next': {
                    click: this.onNextButtonClick
                },
				//

                'historicalsetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalsetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalsetting #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});