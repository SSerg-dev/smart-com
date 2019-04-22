Ext.define('App.util.core.ObservableMap', {
    mixins: {
        observable: 'Ext.util.Observable'
    },

    defaultData: null,

    constructor: function (data) {
        this.callParent(arguments);
        this.mixins.observable.constructor.call(this);

        this.addEvents(
            'edit',
            'reject',
            'commit',
            'clear'
        );

        this.initData(data);
    },

    initData: function (data) {
        var fields = Ext.Object.getKeys(this.defaultData);
        this.data = App.Map.pull(Ext.apply(this.defaultData, data), fields);
    },

    get: function (name) {
        return this.changes && this.changes.hasOwnProperty(name)
            ? this.changes[name]
            : this.data[name];
    },

    set: function (name, value) {
        if (this.data.hasOwnProperty(name)) {
            var oldValue = this.get(name);

            if (!this.changes) {
                this.changes = {};
            }

            this.changes[name] = value;
            this.fireEvent('edit', this, name, value, oldValue);
        } else {
            console.warn('Can not set property with name: ', name);
        }
    },

    commit: function () {
        Ext.apply(this.data, this.changes);
        delete this.changes;
        this.fireEvent('commit', this);
    },

    reject: function () {
        delete this.changes;
        this.fireEvent('reject', this);
    },

    clear: function () {
        this.initData();
        this.fireEvent('clear', this);
    }

});