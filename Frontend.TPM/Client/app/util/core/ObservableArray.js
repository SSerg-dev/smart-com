Ext.define('App.util.core.ObservableArray', {
    mixins: {
        observable: 'Ext.util.Observable'
    },

    constructor: function () {
        this.callParent(arguments);
        this.mixins.observable.constructor.call(this, arguments);

        this.addEvents(
            'add',
            'addFull',
            'remove',
            'removeFull',
            'commit',
            'reject',
            'clear'
        );

        this.entries = Ext.create('Ext.util.MixedCollection', {
            allowFunctions: false,
            getKey: this.getEntryKey
        });
        this.removedEntries = Ext.create('Ext.util.MixedCollection', {
            allowFunctions: false,
            getKey: this.getEntryKey
        });
    },

    add: function (items) {
        var entries = this.entries;

        Ext.Array.from(items).forEach(function (entry) {
            if (!entries.contains(entry)) {
                entry.phantom = true;
                entries.add(entry);

                this.onAdd(entry);
                this.fireEvent('add', this, entry);
            }
        }, this);
    },
    addFull: function (items) {
        this.fireEvent('addFull', this, items);
    },
    removeFull: function (items) {
        this.fireEvent('removeFull', this, items);
    },
    remove: function (items) {
        var entries = this.entries,
            removedEntries = this.removedEntries;

        Ext.Array.from(items).forEach(function (entry) {
            entries.remove(entry);

            if (entries.contains(entry) && !entry.phantom) {
                removedEntries.add(entry);
            }

            this.onRemove(entry);
            this.fireEvent('remove', this, entry);
        }, this);
    },

    commit: function () {
        var removedEntries = this.removedEntries,
            entries = this.entries;

        removedEntries.clear();
        entries.each(function (entry) {
            entry.commit();
            delete entry.phantom;
        });

        this.onCommit();
        this.fireEvent('commit', this);
    },

    reject: function () {
        var removedEntries = this.removedEntries,
            entries = this.entries;

        entries.removeAll(entries.getRange().filter(function (entry) {
            return entry.phantom;
        }));
        entries.addAll(removedEntries.getRange());
        entries.each(function (entry) {
            entry.reject();
        });

        this.onReject();
        this.fireEvent('reject', this);
    },

    clear: function () {
        var removedEntries = this.removedEntries,
            entries = this.entries;

        removedEntries.clear();
        entries.clear();

        this.onClear();
        this.fireEvent('clear', this);
    },

    getEntryKey: function (item) {
        return item.get('id');
    },

    onClear: Ext.emptyFn,
    onReject: Ext.emptyFn,
    onCommit: Ext.emptyFn,
    onRemove: Ext.emptyFn,
    onAdd: Ext.emptyFn

});