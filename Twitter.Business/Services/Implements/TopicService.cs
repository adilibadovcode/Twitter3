using AutoMapper;
using Twitter.Business.Dtos.TopicDtos;
using Twitter.Business.Exceptions.Common;
using Twitter.Business.Exceptions.Topic;
using Twitter.Business.Repositories.Interface;
using Twitter.Business.Services.Interface;
using Twitter.Core.Entities;

namespace Twitter.Business.Services.Implements;
public class TopicService : ITopicService
{
    ITopicRepository _repo { get; }
    IMapper _mapper { get; }
    public TopicService(ITopicRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(TopicCreateDto dto)
    {
        if (await _repo.IsExistAsync(r => r.Name.ToLower() == dto.Name.ToLower()))
            throw new TopicExistException();
        await _repo.CreateAsync(_mapper.Map<Topic>(dto));
        await _repo.SaveAsync();
    }

    public IEnumerable<TopicListItemDto> GetAll()
     => _mapper.Map<IEnumerable<TopicListItemDto>>(_repo.GetAll());

    public async Task<TopicDetailDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException();
        var data = await _repo.GetByIdAsync(id);
        if (data == null) throw new NotFoundException<Topic>();
        return _mapper.Map<TopicDetailDto>(data);

    }

    public async Task RemoveAsync(int id)
    {
        var data =await _checkId(id);
        _repo.Remove(data);
        await _repo.SaveAsync();

    }
    public async Task UpdateAsync(int id, TopicUpdateDto dto)
    {
        var data = await _checkId(id);
        data = _mapper.Map(dto, data);
        await _repo.SaveAsync();
    }
    async Task<Topic> _checkId(int id)
    {
        if (id <= 0) throw new ArgumentException();
        var data = await _repo.GetByIdAsync(id, false);
        if (data == null) throw new NotFoundException<Topic>();
        return data;
    }
}
