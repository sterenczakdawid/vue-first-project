const pl = {
  title: "Panel zarządcy",
  home: "Strona główna",
  server: "Serwer",
  servers: "Serwery",
  serverDetails: "Szczegóły serwera",
  apps: "Aplikacje",
  app: "Aplikacja",
  appAdd: "Aplikację",
  tasks: "Zadania",
  task: "Zadanie",
  search: "Szukaj",
  selectServer: "Wybierz serwer",
  selectApp: "Wybierz aplikację",
  addServer: "Dodaj nowy serwer",
  addApp: "Dodaj aplikację",
  addTask: "Dodaj zadanie",
  headers: {
    name: "Nazwa",
    created: "Utworzono",
    edited: "Edytowano",
    actions: "Akcje",
  },
  details: "szczegóły",
  buttons: {
    edit: "Edytuj ",
    delete: "Usuń",
    back: "Powrót",
    cancel: "Anuluj",
    submit: "Zapisz",
    confirm: "Potwierdź",
  },
  appAttached: "Ta aplikacja jest podłączona do serwera",
  taskAttachedServer: "To zadanie jest podłączone do sewera",
  taskAttachedApp: "To zadanie jest podłączone do aplikacji",
  taskNotAttachedApp: "To zadanie nie jest podłączone do żadnej aplikacji",
  addNew: "Dodaj ",
  errors: {
    nameRequired: "Nazwa jest wymagana",
    nameTooShort: "Nazwa powinna mieć długość minimum 3 znaków",
    taskAttachedToServer: "Zadanie musi być podpięte do serwera",
    appAttachedToServer: "Aplikacja musi być podpięta do serwera",
    serverNameAlreadyExists: "Serwer o takiej nazwie już istnieje",
    appNameAlreadyExists: "Aplikacja o takiej nazwie już istnieje",
    taskNameAlreadyExists: "Zadanie o takiej nazwie już istnieje",
  },
  noDataText: {
    noServer: "Wybierz serwer, aby zobaczyć podpięte zadania",
    noServerTasks: "Wybrany serwer nie ma dostępnych zadań",
    noServerApp: "Wybierz serwer, aby zobaczyć podpięte aplikacje",
    noServerApps: "Wybrany serwer nie ma dostępnych aplikacji",
  },
  deleteConfirm: "Czy na pewno chcesz usunąć",
  warning: "Uwaga!",
  deletingApp:
    "Usunięcie aplikacji spowoduje odpięcie od niej wszystkich jej zadań. Zadania te pozostaną niepodpięte do żadnej aplikacji",
  deletingServer:
    "Usunięcie serwera spowoduje automatyczne usunięcie wszystkich podpiętych do niego aplikacji oraz zadań",
  itemsAllText: "Wszystkie",
  itemsPerPage: "Wiersze na stronę",
  of: "z",
  snackbars: {
    saveSuccess: "Pomyślnie zapisano",
    saveError: "Wystąpił błąd przy zapisie",
    deleteSuccess: "Pomyślnie usunięto",
    deleteError: "Wystąpił błąd przy usuwaniu",
  },
};

export default pl;
