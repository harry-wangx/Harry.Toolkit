using System;

namespace Harry.Factory
{
    public interface IFactory<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// ��������ʵ��
        /// </summary>
        /// <param name="name">��������</param>
        bool TryCreate(string name, out T value);

        /// <summary>
        /// ����ṩ�� <see cref="IProvider<T>"/> ������.
        /// </summary>
        /// <param name="provider">�ṩ�� <see cref="IProvider<T>"/>.</param>
        void AddProvider(IProvider<T> provider);
    }
}
