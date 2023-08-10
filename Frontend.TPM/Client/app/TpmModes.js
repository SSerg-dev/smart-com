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
        let tpmModeStore = Ext.data.StoreManager.lookup('tpmModeStore');
        if (Ext.isEmpty(tpmModeStore)) {
            tpmModeStore = Ext.create('App.store.tpm.mode.Mode');
        }
        return tpmModeStore;
    },
    getSettingStore: function() {
        let settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        if (Ext.isEmpty(settingStore)) {
            settingStore = Ext.create('App.store.core.settinglocal.SettingLocalStore');
            settingStore.load();
        }
        return settingStore;
    },
    setMode: function(modeId) {
        let settingStore = this.getSettingStore();
        settingStore.load();
        let mode = settingStore.findRecord('name', 'mode');
        if (!Ext.isEmpty(mode)) {
            mode.set('value', modeId);
        } else {
            settingStore.add({ name: 'mode', value: modeId });
        }
        settingStore.sync();
    },
    getSelectedModeItem: function() {
        let settingStore = this.getSettingStore();
        return settingStore.findRecord('name', 'mode');
    },
    getTpmModeById: function(modeId) {
        let mode = this.getTpmModeStore().findRecord('id', modeId);
        return mode ? mode.data : null;
    },
    getSelectedMode: function() {
        let modeId = this.getSelectedModeId();
        return this.getTpmModeById(modeId);
    },
    getSelectedModeId: function() {
        let mode = this.getSelectedModeItem();
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