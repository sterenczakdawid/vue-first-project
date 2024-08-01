export const ItemDetailsMixin = {
  data() {
    return {
      id: this.$route.params.id,
      editedItem: {},
      dialogEdit: false,
      dialogDelete: false,
    };
  },
  methods: {},
};
