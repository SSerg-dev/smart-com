TpmModes = {
    Prod: {
        id: 0,
        alias: 'Current',
        text: 'Production'
    },
    RS: {
        id: 1,
        alias: 'RS',
        text: 'Rolling Scenario'
    },
    RA: {
        id: 2,
        alias: 'RA',
        text: 'Resource Allocation'
    },
    getTpmModeStore: function() {
        var tpmModeStore = Ext.data.StoreManager.lookup('tpmModeStore');
        if (Ext.isEmpty(tpmModeStore)) {
            tpmModeStore = Ext.create('App.store.tpm.mode.Mode');
        }
        return tpmModeStore;
    },
    getSettingStore: function() {
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        if (Ext.isEmpty(settingStore)) {
            settingStore = Ext.create('App.store.core.settinglocal.SettingLocalStore');
            settingStore.load();
        }
        return settingStore;
    },
    setMode: function(modeId) {
        var settingStore = this.getSettingStore();
        settingStore.load();
        var mode = settingStore.findRecord('name', 'mode');
        if (!Ext.isEmpty(mode)) {
            mode.set('value', modeId);
        } else {
            settingStore.add({ name: 'mode', value: modeId });
        }
        settingStore.sync();
    },
    getSelectedModeItem: function() {
        var settingStore = this.getSettingStore();
        return settingStore.findRecord('name', 'mode');
    },
    getTpmModeById: function(modeId) {
        var mode = this.getTpmModeStore().findRecord('id', modeId);
        return mode ? mode.data : null;
    },
    getSelectedMode: function() {
        var modeId = this.getSelectedModeId();
        return this.getTpmModeById(modeId);
    },
    getSelectedModeId: function() {
        var mode = this.getSelectedModeItem();
        return mode ? mode.data.value : this.Prod.id;
    },
    isRsRaMode: function(mode) {
        if (typeof mode === 'string') {
            mode = Ext.isEmpty(mode) ? this.getSelectedMode().alias : mode;
            return mode === this.RS.alias || mode === this.RA.alias;
        } else {
            mode = Ext.isEmpty(mode) ? this.getSelectedModeId() : mode;
            return mode === this.RS.id || mode === this.RA.id;
        }
    },
    isProdMode: function(mode) {
        if (typeof mode === 'string') {
            mode = Ext.isEmpty(mode) ? this.getSelectedMode().alias : mode;
            return mode === this.Prod.alias;
        } else {
            mode = Ext.isEmpty(mode) ? this.getSelectedModeId() : mode;
            return mode === this.Prod.id;
        }
    },
    isRsMode: function(mode) {
        if (typeof mode === 'string') {
            mode = Ext.isEmpty(mode) ? this.getSelectedMode().alias : mode;
            return mode === this.RS.alias;
        } else {
            mode = Ext.isEmpty(mode) ? this.getSelectedModeId() : mode;
            return mode === this.RS.id;
        }
    },
    isRaMode: function(mode) {
        if (typeof mode === 'string') {
            mode = Ext.isEmpty(mode) ? this.getSelectedMode().alias : mode;
            return mode === this.RA.alias;
        } else {
            mode = Ext.isEmpty(mode) ? this.getSelectedModeId() : mode;
            return mode === this.RA.id;
        }
    }
}