<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title class="font-weight-regular"
        >{{ dialogTitle }}{{ itemType }}</v-card-title
      >
      <v-card-text>
        <v-form
          class="d-flex flex-column px-3"
          ref="form"
          v-model="valid"
          lazy-validation
        >
          <v-text-field
            v-model.trim="formData.name"
            :rules="nameRules"
            label="Name"
            required
          ></v-text-field>
          <v-select
            v-model="formData.serverId"
            :items="servers"
            :rules="serversRules"
            label="Server"
            item-text="name"
            item-value="id"
            required
          ></v-select>
          <v-select
            v-model="formData.appId"
            :items="apps"
            label="Application"
            item-text="name"
            item-value="id"
          ></v-select>
          <div class="align-self-end">
            <v-btn @click="close"> Cancel </v-btn>
            <v-btn class="success" @click="submit">Submit</v-btn>
          </div>
        </v-form>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  emits: ["submit", "close"],
  props: {
    dialog: { type: Boolean, required: true },
    mode: { type: String, required: true },
    itemType: { type: String, required: true },
    initialData: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      valid: true,
      formData: { ...this.initialData },
      nameRules: [
        (v) => !!v.trim() || "Name is required",
        (v) => v.trim().length >= 3 || "Name must have at least 3 characters",
      ],
      serversRules: [(v) => v > 0 || "Task has to be attached to a server"],
      select: null,
    };
  },
  computed: {
    dialogTitle() {
      return this.mode === "add" ? "Add new " : "Edit ";
    },
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
  },
  methods: {
    close() {
      this.$refs.form.resetValidation();
      this.$emit("close");
    },
    submit() {
      this.$emit("submit", this.formData);
    },
  },
  watch: {
    initialData: {
      handler(newData) {
        this.formData = { ...newData };
      },
      deep: true,
    },
  },
};
</script>
