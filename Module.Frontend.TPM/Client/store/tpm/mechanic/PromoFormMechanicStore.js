Ext.define('App.store.tpm.mechanic.PromoFormMechanicStore', {
    extend: 'App.store.core.SimpleStore',
    alias: 'store.promoformmechanicstore',
    autoLoad: false,
    model: 'App.model.tpm.mechanic.Mechanic',
    extendedFilter: {
        xclass: 'App.ExtFilterContext',
        supportedModels: [{
            xclass: 'App.ExtSelectionFilterModel',
            model: 'App.model.tpm.mechanic.Mechanic',
            modelId: 'efselectionmodel'
        }]
    }
});
