Ext.define('App.util.core.Selectable', {

    isSelectableView: true,
    selectedUI: 'blue-selectable-panel',

    statics: {
        activeView: null,
        selectionHistory: [],
        selectionHistoryDepth: 10,

        setActiveView: function (newActiveView) {
            var lastActiveView = this.activeView;

            if (newActiveView && newActiveView.fireEvent('beforeviewselectionchange', lastActiveView, newActiveView) === false) {
                return;
            }

            if (lastActiveView !== newActiveView) {
                if (lastActiveView && !(lastActiveView.isDestroyed || lastActiveView.destroying)) {
                    lastActiveView.isSelected = false;
                    lastActiveView.setUI(lastActiveView.previousUI);
                }

                this.activeView = newActiveView;
                this.pushToHistory(newActiveView);

                if (newActiveView && !(newActiveView.isDestroyed || newActiveView.destroying)) {
                    newActiveView.isSelected = true;
                    newActiveView.setUI(newActiveView.frame ? newActiveView.selectedUI + '-framed' : newActiveView.selectedUI);
                    newActiveView.fireEvent('viewselectionchange', lastActiveView, newActiveView);
                }
            }
        },

        pushToHistory: function (view) {
            this.selectionHistory.push(view);
            if (this.selectionHistory.length > this.selectionHistoryDepth) {
                this.selectionHistory.shift();
            }
        },

        setActivePreviousView: function () {
            var view;

            this.selectionHistory.pop();
            view = this.selectionHistory.pop();

            while (this.selectionHistory.length > 0 && (!view || (view && (view.isDestroyed || view.destroying)))) {
                view = this.selectionHistory.pop();
            }

            this.setActiveView(view);
        }

    },

    constructor: function () {
        this.callParent(arguments);
        this.isSelected = false;
        this.previousUI = this.ui;
        this.addEvents('beforeviewselectionchange', 'viewselectionchange');
    },

    afterRender: function () {
        var el = this.getEl();

        if (el) {
            el.dom.addEventListener('focus', Ext.bind(this.onElementFocus, this), true);
            el.dom.addEventListener('click', Ext.bind(this.onElementFocus, this), true);
            el.dom.addEventListener('touch', Ext.bind(this.onElementFocus, this), true);
            console.log('Focus listener added to:', Ext.getClassName(this));
        }

        if (!this.suppressSelection) {
            App.util.core.Selectable.setActiveView(this);
        }
    },

    onDestroy: function () {
        if (this.isSelected) {
            App.util.core.Selectable.setActivePreviousView();
            console.log('Selectable view destroyed', Ext.getClassName(this));
        }
    },

    onElementFocus: function () {
        debugger;
        App.util.core.Selectable.setActiveView(this);
        console.log('Element focused', Ext.getClassName(this), arguments);
    }

});