export const CrudMixin = {
  data() {
    return {
      search: "",
      dialog: false,
      mode: "add",
      dialogDelete: false,
      editedIndex: -1,
      editedItem: {
        name: "",
        created: null,
        edited: null,
        tasksIds: [],
        appsIds: [],
      },
      defaultItem: {
        name: "",
        created: null,
        edited: null,
        tasksIds: [],
        appsIds: [],
      },
    };
  },
  computed: {
    items() {
      return this.$store.getters[`modules/${this.module}/${this.module}`];
    },
    editedItemName() {
      return this.editedItem.name;
    },
  },
  methods: {
    handleClick(item) {
      this.$router.push(this.$route.path + "/" + item.id);
      console.log(item);
    },
    editItem(item) {
      this.mode = "edit";
      this.editedIndex = this.items.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },
    deleteItem(item) {
      this.editedIndex = item.id;
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch(
        `modules/${this.module}/remove${this.itemType}`,
        this.editedIndex
      );
      this.closeDelete();
    },
    openDialog(mode) {
      this.mode = mode;
      this.dialog = true;
    },
    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    closeDialog(isDelete) {
      if (isDelete) {
        this.deleteDialog = false;
      } else {
        this.dialog = false;
      }
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    submit(formData) {
      const data = {
        ...formData,
        edited: new Date().toLocaleString().slice(0, -3),
      };
      if (!formData.created) {
        data.created = new Date().toLocaleString().slice(0, -3);
      }
      if (this.editedIndex > -1) {
        this.$store.dispatch(`modules/${this.module}/update${this.itemType}`, {
          index: this.editedIndex,
          item: data,
        });
      } else {
        this.$store.dispatch(
          `modules/${this.module}/add${this.itemType}`,
          data
        );
      }
      this.dialog = false;
    },
    loadItems() {
      // console.log("loadItems w crud mixin wywolane");
      this.$store.dispatch("modules/apps/loadApps");
      // this.$store.dispatch("modules/tasks/loadTasks", {
      //   page: this.page,
      //   pageSize: this.pageSize,
      // });
      // this.$store.dispatch("modules/tasks/loadTasks");
      this.$store.dispatch("modules/servers/loadServers");
    },
  },
  created() {
    this.loadItems();
  },
};
