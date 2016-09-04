namespace SlightLibrary.Bases {

    public interface IWorker {

        bool IsRunning {
            get;
        }

        void Start(bool loop = false);

        void RequestStop();

        void Join();

    }

}
