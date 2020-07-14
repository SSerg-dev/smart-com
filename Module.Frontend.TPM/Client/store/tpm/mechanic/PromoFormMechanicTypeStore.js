Ext.define('App.store.tpm.mechanic.MechanicTypeStore', {
    extend: 'App.store.core.SimpleStore',
    autoLoad: false,
    alias: 'store.promoformmechanictypestore',
    model: 'App.model.tpm.mechanictype.MechanicTypeForClient',
    extendedFilter: {
        xclass: 'App.ExtFilterContext',
        supportedModels: [{
            xclass: 'App.ExtSelectionFilterModel',
            model: 'App.model.tpm.mechanictype.MechanicTypeForClient',
            modelId: 'efselectionmodel'
        }]
    }
});
